# API - Roadmap

Versions:

- v1.0 - The Pig - MVP
- v2.0 - Empty Throttle
- v3.0 - Bonobo Island

All versions must be released:

- With all features like described below.
- With a minimum of 90% of code coverage.
- 0 warnings on release.
- Low technical debt: no more than 10 TODO's
  - Workarounds
  - Smelly code

## v1.0 Roadmap - The Pig - MVP

On the MVP the application must have the following features:

1. User authentication and authorization.
1. Profile managment
1. Module management
1. Course management
1. Module schedule management
1. Attendance management

### 0.1 - Users

- Role management ✔
- User Registration ✔
  - Role assign ✔
- User login ✔
- User authorization ✔

### 0.2 - Profile management

- Users can manage their personal information. ✔
  - Personal information will be stored on the Persons table, which can be deleted at any time by the user. ✔
  - User <=> Person relationship should be encrypted. ✔

### 0.3 - Module management

- Administrators can manage modules
- Adminsitrators can assign or remove professors to a module.
- Administrators and professors can manage the study plan of a course.

### 0.4 - Course management

- Administrators can create courses.
- Administrators can assign or remove modules to a course.
- Administrators can add or remove students to a course.

### 0.5 - Module schedule management

- Administrators can manage module schedules.

### 0.6 - Attendance management

- Attendance must have change history.
- Administrators can manage attendance for all modules on any dates.
- Professors can freely manage history on the module timeframe.
- Students can consult their attendance history.

### v1.0 - The Pig - RELEASE

## v2.0 Roadmap - Empty Throttle

This milestone should include the following features:

1. Module automatic schedule.
1. Notification management.
1. Student tutors management.

## v3.0 Roadmap - Bonobo Island

This milestone should include the following features:

1. Exams management
1. Student requests.
    - Join to course.
    - Attendance rectification.
    - Notes rectifications.
1. Professor requests.
    - Change schedule.