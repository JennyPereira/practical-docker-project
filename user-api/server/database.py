import motor.motor_asyncio
import env_variables
from pymongo.errors import DuplicateKeyError

client = motor.motor_asyncio.AsyncIOMotorClient(env_variables.MONGODB_CONNECTION_URI)
db = client.test
users_collection = db.get_collection("users")
try:
    users_collection.create_index('email', unique=True)
except Exception as e:
    print("ERROR: ", e)

def user_helper (user) -> dict:
    return {
        "email": str(user["email"]),
        "password": str(user["password"])
    }