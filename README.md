# EduTrack - Course Catalog API

An ASP.NET Core API for managing a course catalog.

## Features

- RESTful API for course management
- In-memory data storage
- CRUD operations for courses
- Advanced search functionality
- Input validation
- xUnit tests

## Project Structure

- **Models**: Course class with validation attributes
- **Services**: ICourseService interface and CourseService implementation
- **Controllers**: CoursesController with RESTful endpoints
- **Tests**: xUnit tests for the API

## API Endpoints

### Get All Courses
```
GET /api/courses
```

### Get Course by ID
```
GET /api/courses/{id}
```

### Search Courses
```
GET /api/courses/search?title={title}&category={category}&minPrice={minPrice}&maxPrice={maxPrice}
```

All search parameters are optional. Examples:
- `/api/courses/search?title=programming`
- `/api/courses/search?category=web`
- `/api/courses/search?minPrice=20&maxPrice=50`
- `/api/courses/search?title=intro&category=programming&maxPrice=100`

### Create New Course
```
POST /api/courses
```

Request body:
```json
{
  "title": "New Course Title",
  "description": "Course description",
  "price": 49.99,
  "category": "Programming"
}
```

### Update Course
```
PUT /api/courses/{id}
```

Request body:
```json
{
  "courseId": 1,
  "title": "Updated Course Title",
  "description": "Updated description",
  "price": 59.99,
  "category": "Programming"
}
```

### Delete Course
```
DELETE /api/courses/{id}
```

## Validation

- Course title cannot be empty
- Price must be non-negative
- Required fields: Title, Price, Category

## Running the Project

1. Make sure you have .NET 6 or 7 installed
2. Clone the repository
3. Run the project: `dotnet run`
4. Access the Swagger UI at `/swagger` to test the API

## Running Tests

```
dotnet test
```
