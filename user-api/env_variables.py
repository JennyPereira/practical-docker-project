import os
from dotenv import load_dotenv

load_dotenv()

MONGODB_CONNECTION_URI = os.getenv('MONGODB_CONNECTION_URI')
AUTH_API_ADDRESS = os.getenv('AUTH_API_ADDRESS')