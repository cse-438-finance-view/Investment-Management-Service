# Investment Management Service

A microservice-based user and investment management system that provides secure authentication and handles investment data.

## Overview

This service is designed to manage user authentication and investment data using ASP.NET Core and PostgreSQL, with RabbitMQ for event-driven communication.

## Aggregates

### 1. AppUser Aggregate
  - Manages user information and authentication data
  - Communicates user lifecycle changes through domain events
  - Related Entity: `AppUser.cs`

## Domain Events

### 1. UserCreatedEvent
  - Trigger: When a new user is created
  - Content: UserId, Email, Name, Surname
  - Potential Subscribers: Mail Service, analytics service, other microservices

### 2. UserUpdatedEvent
  - Trigger: When user profile information is modified

### 3. UserLoginEvent
  - Trigger: When user successfully logs into the system

### 4. UserDeletedEvent
  - Trigger: When user account is deleted or deactivated
    
### 5. UserLockedOutEvent
  - Trigger: When user account is locked due to failed login attempts


## Planned Additions

### Investment Aggregate
Will manage investment assets

- InvestmentCreatedEvent: When a new investment is recorded
- InvestmentUpdatedEvent: When investment details are modified
