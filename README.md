
# Practical Docker Project

After finishing the Docker and Kubernetes course, I decided to use a class project and create an app that integrates three programming languages:: 
 
- C#
- Python
- JavaScript (NodeJs, Express)

These languages working with each other.  I integrated JWT into all three of them.

## Acknowledgements

 - [Docker & Kubernetes: The Practical Guide 2024 Edition](https://www.udemy.com/course/docker-kubernetes-the-practical-guide/?couponCode=ST15MT31224)

## Environment Variables

To run this project, you will need to add the following environment variables to your .env file

`MONGODB_CONNECTION_URI`

`AUTH_API_ADDRESS`

`TOKEN_KEY`

## Run Locally

Clone the project

```bash
  git clone https://github.com/JennyPereira/practical-docker-project.git
```

Go to the project directory

```bash
  cd docker-microservices
```

Start the server

```bash
  docker-compose up -d
```

Stop the server

```bash
  docker-compose down --rmi all
```

## API Reference

#### Sign up

```http
  POST /signup
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `email` | `string` | **Required**. Your desired email |
| `password` | `string` | **Required**. Your password |

#### Login

```http
  POST /login
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `email` | `string` | **Required**. Your desired email |
| `password` | `string` | **Required**. Your password |


#### Create task

```http
  POST /api/tasks
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Required**. Your API key |


#### Delete task

```http
  DELETE /api/tasks/${id}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Required**. Your API key |
| `id`      | `string` | **Required**. Id of task to be deleted |
