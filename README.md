# MAUI RSVP App

A multi-page .NET MAUI event management and RSVP application developed as part of Mobile App Development II.

The project has grown from a navigation-focused class assignment into a more complete application with user authentication state, event creation, RSVP workflows, local SQLite persistence, validation rules, and structured data-access logic.

## Features

- User login and authenticated user state
- Event browsing and event details
- Event creation with date, time, location, RSVP deadline, and attendee-limit fields
- RSVP submission and confirmation
- Filtering events associated with the logged-in user
- SQLite-backed local persistence for users, events, and RSVP records
- Duplicate-RSVP prevention
- RSVP deadline enforcement
- Maximum-attendee validation
- Multi-page navigation and model data passed between pages

## Tech Stack

- C#
- .NET MAUI
- XAML
- SQLite
- sqlite-net-pcl
- Visual Studio
- Git and GitHub

## What This Project Demonstrates

This project demonstrates growing experience with:

- Building multi-page applications with .NET MAUI and XAML
- Separating models, data-access logic, and UI responsibilities
- Working with asynchronous database operations
- Designing relational data around users, events, and RSVP records
- Maintaining logged-in user state across application workflows
- Validating user input and enforcing event rules
- Debugging and extending an application across multiple development milestones
- Using Git and GitHub to track iterative development

## Architecture

The application separates responsibilities across UI pages, model classes, and a local data-access layer. SQLite is used to persist user, event, and RSVP data across application sessions.

The data layer creates and manages tables for users, events, and RSVP records and provides asynchronous methods used by the application UI.

## Development Progress

The application has been developed incrementally across course milestones. Recent development includes:

- SQLite-backed event and RSVP persistence
- User authentication and current-user state
- Event and RSVP validation
- Duplicate-RSVP prevention
- RSVP deadline and attendee-capacity rules
- Expanded event-management workflows

## Application Screenshots

| Login | Event List | Event Details & RSVP |
|---|---|---|
| User entry point for the application. | Browse available events and open event details. | Review event information, attendee status, RSVP deadline, and submit a response. |
| <img src="https://github.com/user-attachments/assets/eabbdcb2-cf64-423f-8f01-556e88cbf7a4" alt="RSVP App Login Page" width="260"> | <img src="https://github.com/user-attachments/assets/036d3930-bbd6-45a4-a7b2-1602220f372b" alt="RSVP Event List" width="260"> | <img src="https://github.com/user-attachments/assets/6472932e-b721-46d2-9169-be535f15de25" alt="RSVP Event Details and RSVP Form" width="260"> |

## Current Status

This is an actively developed academic project rather than a production-ready commercial application. The current version demonstrates functional application workflows, local persistence, authentication state, validation, and event/RSVP management.

## Future Improvements

Potential future improvements include:

- Additional UI refinement
- Expanded validation and error handling
- Calendar integration
- Additional event-management controls
- Remote/cloud-backed data storage
- Automated testing
