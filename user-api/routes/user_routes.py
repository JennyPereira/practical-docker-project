from fastapi import FastAPI, Body, APIRouter
from fastapi.encoders import jsonable_encoder
from ..user_app import app
from ..model import user_model

@app.post("/signup/")
async def create_user(user: user_model.UserModel = Body(...)):
    return user

@app.post("/login/")
async def verify_user():
    print()