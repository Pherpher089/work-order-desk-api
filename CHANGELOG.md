# Changelog

## [Unreleased]

### Added 4/10/26

- Screenshot for README

### Changed 4/10/26

- Configured JSON enum serialization to support string enum values in requests

### Added 4/10/26

- Delete Work Order use case (application layer)
- DELETE /work-orders/{id} endpoint

### Changed 4/10/26

- Expanded work order repository abstraction to support delete operations

### Added 4/9/26

- Get Work Order by ID use case (application layer)
- GET /work-orders/{id} endpoint
- Work order details response model

### Fixed 4/9/26

- Dependency injection registration for GetWorkOrderByIdHandler

### Changed 4/9/26

- Improved API consistency for work order retrieval

### Changed 4/8/26

- README update

### Added 4/6/26

- CORS policy for local frontend development

### Added 4/6/26

- Global exception handling middleware
- Consistent HTTP error responses for domain validation failures

### Changed 4/6/26

- API now returns 400 Bad Request for invalid input instead of 500 errors

### Added 3/30/26

- Create Work Order use case (handler + command + result)
- Create Work Order API endpoint (POST /work-orders)
- Request/response DTOs for work order creation

### Added 3/18/26

- SQLite database created from initial schema

### Added 3/18/26

- Added EF configuration
- Created initial DB migration

### Added 3/2/26

- WorkOrder domain model with status trasitions and audit time stamp
- User entity + Userid

### Added 2/24/26

- Initial solution structure
- Layered architecture (Domain / Application / Infrastructure / API)
- EF Core SQLite setup
