# Changelog

## [Unreleased] 4/8/26

### Changed

- README update

# Changelog

## [Unreleased] 4/6/26

### Added

- CORS policy for local frontend development

## [Unreleased]

### Added

- Global exception handling middleware
- Consistent HTTP error responses for domain validation failures

### Changed

- API now returns 400 Bad Request for invalid input instead of 500 errors

## [Unreleased] 3/30/26

- Create Work Order use case (handler + command + result)
- Create Work Order API endpoint (POST /work-orders)
- Request/response DTOs for work order creation

## [Unreleased] 3/18/26

- SQLite database created from initial schema

# Changelog

## [Unreleased] 3/18/26

- Added EF configuration
- Created initial DB migration

## [Unreleased] 3/2/26

### Added

- WorkOrder domain model with status trasitions and audit time stamp
- User entity + Userid

## [Unreleased] 2/24/26

### Added

- Initial solution structure
- Layered architecture (Domain / Application / Infrastructure / API)
- EF Core SQLite setup
