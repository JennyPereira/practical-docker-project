from fastapi import APIRouter, Body, HTTPException
from fastapi.encoders import jsonable_encoder

from controllers.user_actions import (
    add_user,
    verify_user,
    get_logs
)

from model.user_model import (
    ErrorResponseModel,
    ResponseModel,
    UserSchema,
    UpdateUserModel
)

router = APIRouter()

@router.post("/signup/", response_description="User data add into the database")
async def add_user_data(userSch: UserSchema = Body(...)):
    user = jsonable_encoder(userSch)
    new_user = await add_user(user)
    if new_user:
        return ResponseModel(new_user, "User added succesfully")
    #return ErrorResponseModel("New error", 500, "Failed create user")
    raise HTTPException(status_code=500, detail="Failed create user")


@router.post("/login/")
async def verify_user_data(userSch: UserSchema = Body(...)):
    user = jsonable_encoder(userSch)
    verifying = await verify_user(user)

    return ResponseModel(verifying, "Login with User")
    
    #if verifying == 422 or verifying == 500:
    #    return ErrorResponseModel("Verifying user", 402, "Failed to find and verify user for provided credentials.")
    #else:
    #    print("SI ENTRA")
    #    return ResponseModel(verifying, "Login with User")
    

@router.get("/logs/")
async def getLogs():
    dataArr = await get_logs()
    return ResponseModel(dataArr, "Get logs")