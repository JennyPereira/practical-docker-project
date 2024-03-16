from server.database import user_helper, users_collection
from model.user_model import ErrorResponseModel
from fastapi import HTTPException
import requests, json
from bson.objectid import ObjectId
from datetime import datetime
import os
import chardet
import re
import env_variables

def validate_email(email):
    pattern = r'^[\w\.-]+@[\w\.-]+\.\w+$'
    return bool(re.match(pattern, email))


async def getHashedPassword(password):
    try:
        response = requests.get(f'http://{env_variables.AUTH_API_ADDRESS}/hashed-pw/{password}')
        print("1. ", response.text)
        return response.text;
    except Exception as err:
        print("Error ", err)
        return None

async def getTokenForUser(password, hashedPassword, uid):
    try:
        response = requests.post(f'http://{env_variables.AUTH_API_ADDRESS}/token', json={
            "password": password,
            "hashedPassword": hashedPassword,
            "userId": str(uid)
        })
        res = json.loads(response.text)
        return res["token"]
    except Exception as err:
        raise HTTPException(status_code=500, detail="Something Wrong")


async def writeLogs(email):
    now = datetime.now()
    date_today = now.strftime("%d-%m-%Y")
    log_entry = date_today + ' - ' + email
    file_path = "./users/users-log.txt"
    if not os.path.exists(file_path):
        with open(file_path, "w") as new_file:
            new_file.write(log_entry)
    else:
        with open(file_path, "a+") as file:
            file.seek(0)
            lines = file.readlines()

            file.write(f"\n{log_entry}")


async def add_user(user_data: dict) -> dict:
    #email_name = user_data.match_info.get("email")
    try:
        if not user_data['password'] or len(user_data['password'].strip()) < 7:
            raise HTTPException(status_code=422, detail="Invalid email or password.")
        else:
            passw = user_data['password']
            transformPassword = await getHashedPassword(passw)
            passhedPassword = json.loads(transformPassword)["hashed"]
        
        newUser = {"email": user_data["email"],
                   "password": passhedPassword,}
        
        user = await users_collection.insert_one(newUser)
        await writeLogs(user_data["email"])
        new_user = await users_collection.find_one({"_id": user.inserted_id})
        return user_helper(new_user)
    except Exception:
        raise HTTPException(status_code=422, detail="Failed to create user")
        #return None
    
    
async def verify_user(user_data: dict) -> dict:
    existingUser = await users_collection.find_one({"email": user_data["email"]})
    if existingUser:
        token = await getTokenForUser(user_data["password"], existingUser["password"], existingUser["_id"])
        res_token = {"token": token, "userId": str(existingUser["_id"])}
        return res_token
    else:
        raise HTTPException(status_code=404, detail="Not Found User")
        #return 422


async def get_logs():
    file = open("./users/users-log.txt", "r")
    content = file.readlines()
    file.close()
    return {"logs" : content}