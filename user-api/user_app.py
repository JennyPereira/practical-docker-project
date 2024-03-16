from fastapi import FastAPI, Body
from fastapi.middleware.cors import CORSMiddleware
from typing_extensions import Annotated
from pydantic.functional_validators import BeforeValidator
from routes.user import router as UserRouter

import motor.motor_asyncio

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(UserRouter, tags=["User"])

#PyObjectId = Annotated[str, BeforeValidator]
###
#@app.get("/")
#async def main():
#    return {"message": "Hello World"}