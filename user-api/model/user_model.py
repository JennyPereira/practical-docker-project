from pydantic import BaseModel, EmailStr, Field
from typing import Optional

class UserSchema(BaseModel):
    email: EmailStr = Field(...)
    password: str = Field(..., encoding='utf-8')

    class Config:
        schema_extra = {
            "example": {
                "email": "jdoe@x.edu.ng",
                "password": "secretisimo"
            }
        }

class UpdateUserModel(BaseModel):
    email: Optional[EmailStr]
    password: Optional[str]

    class Config:
        schema_extra = {
            "example": {
                "email": "jdoe2@x.edu.ng",
                "password": "secretisimo2"
            }
        }

def ResponseModel(data, message):
    return {
        "data": [data],
        "code": 200,
        "message": message,
    }

def ErrorResponseModel(error, code, message):
    return {"error": error, "code": code, "message": message}


