    -- TaskManagementSystem Database Creation Script
-- This script creates all tables and inserts sample data

-- ============================================
-- 1. DROP EXISTING TABLES (if they exist)
-- ============================================
IF OBJECT_ID('dbo.TaskNotes', 'U') IS NOT NULL DROP TABLE dbo.TaskNotes;
IF OBJECT_ID('dbo.TaskItem', 'U') IS NOT NULL DROP TABLE dbo.TaskItem;
IF OBJECT_ID('dbo.Employee', 'U') IS NOT NULL DROP TABLE dbo.Employee;
IF OBJECT_ID('dbo.Team', 'U') IS NOT NULL DROP TABLE dbo.Team;

-- ============================================
-- 2. CREATE TABLES
-- ============================================

-- Team Table
CREATE TABLE [dbo].[Team] (
    [TeamId] INT IDENTITY(1,1) PRIMARY KEY,
    [TeamName] NVARCHAR(255) NOT NULL
);

-- Employee Table
CREATE TABLE [dbo].[Employee] (
    [EmployeeId] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [Role] NVARCHAR(50) NOT NULL,
    [TeamId] INT NULL,
    CONSTRAINT [FK_Employee_Team] FOREIGN KEY ([TeamId]) 
        REFERENCES [dbo].[Team]([TeamId]) ON DELETE SET NULL
);

-- TaskItem Table
CREATE TABLE [dbo].[TaskItem] (
    [TaskId] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [DueDate] DATE NOT NULL,
    [IsCompleted] BIT NOT NULL DEFAULT 0,
    [EmployeeId] INT NOT NULL,
    CONSTRAINT [FK_TaskItem_Employee] FOREIGN KEY ([EmployeeId]) 
        REFERENCES [dbo].[Employee]([EmployeeId]) ON DELETE CASCADE
);

-- TaskNotes Table
CREATE TABLE [dbo].[TaskNotes] (
    [NoteId] INT IDENTITY(1,1) PRIMARY KEY,
    [Content] NVARCHAR(MAX) NOT NULL,
    [CreatedDate] DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    [TaskId] INT NOT NULL,
    CONSTRAINT [FK_TaskNotes_TaskItem] FOREIGN KEY ([TaskId]) 
        REFERENCES [dbo].[TaskItem]([TaskId]) ON DELETE CASCADE
);

-- ============================================
-- 3. CREATE INDEXES
-- ============================================

CREATE INDEX [IX_Employee_TeamId] ON [dbo].[Employee]([TeamId]);
CREATE INDEX [IX_TaskItem_EmployeeId] ON [dbo].[TaskItem]([EmployeeId]);
CREATE INDEX [IX_TaskItem_IsCompleted] ON [dbo].[TaskItem]([IsCompleted]);
CREATE INDEX [IX_TaskNotes_TaskId] ON [dbo].[TaskNotes]([TaskId]);

-- ============================================
-- 4. INSERT SAMPLE DATA
-- ============================================

-- Insert Teams
INSERT INTO [dbo].[Team] ([TeamName]) VALUES
('Engineering'),
('Product Management'),
('Design'),
('Quality Assurance'),
('DevOps');

-- Insert Employees
INSERT INTO [dbo].[Employee] ([Name], [Email], [Role], [TeamId]) VALUES
('Shafik Khatib', 'shafik.khatib@company.com', 'Manager', 1),
('John Smith', 'john.smith@company.com', 'Employee', 1),
('Sarah Johnson', 'sarah.johnson@company.com', 'Employee', 1),
('Michael Brown', 'michael.brown@company.com', 'TeamLead', 2),
('Emily Davis', 'emily.davis@company.com', 'Employee', 2),
('David Wilson', 'david.wilson@company.com', 'Employee', 2),
('Jessica Martinez', 'jessica.martinez@company.com', 'Manager', 3),
('Chris Anderson', 'chris.anderson@company.com', 'Employee', 3),
('Lisa Thompson', 'lisa.thompson@company.com', 'Employee', 3),
('Robert White', 'robert.white@company.com', 'TeamLead', 4),
('Jennifer Harris', 'jennifer.harris@company.com', 'Employee', 4),
('Mark Lewis', 'mark.lewis@company.com', 'Employee', 4),
('Alex Turner', 'alex.turner@company.com', 'Admin', 5),
('Patricia Clark', 'patricia.clark@company.com', 'Employee', 5),
('James Rodriguez', 'james.rodriguez@company.com', 'Employee', 5);

-- Insert Tasks
INSERT INTO [dbo].[TaskItem] ([Title], [Description], [DueDate], [IsCompleted], [EmployeeId]) VALUES
('Complete API Documentation', 'Write comprehensive documentation for REST APIs', CAST('2026-11-15' AS DATE), 0, 2),
('Fix Login Bug', 'Resolve authentication issue in login module', CAST('2026-10-25' AS DATE), 1, 2),
('Code Review for Pull Request #45', 'Review and approve PR #45 from Sarah', CAST('2026-10-24' AS DATE), 1, 2),
('Implement User Dashboard', 'Create responsive user dashboard with charts', CAST('2026-11-30' AS DATE), 0, 2),
('Update Database Schema', 'Add new columns to user and task tables', CAST('2026-10-26' AS DATE), 0, 3),
('Performance Optimization', 'Optimize slow database queries', CAST('2026-11-10' AS DATE), 0, 3),
('Unit Test Coverage', 'Increase unit test coverage to 80%', CAST('2026-11-20' AS DATE), 0, 3),
('Market Research Analysis', 'Analyze competitor product features', CAST('2026-10-28' AS DATE), 1, 5),
('Product Roadmap Planning', 'Create Q4 2026 product roadmap', CAST('2026-11-05' AS DATE), 0, 5),
('User Interview Synthesis', 'Compile feedback from user interviews', CAST('2026-11-15' AS DATE), 0, 5),
('Design System Updates', 'Update color palette and typography', CAST('2026-10-30' AS DATE), 0, 8),
('Prototype Mobile App', 'Create high-fidelity prototype for mobile app', CAST('2026-11-12' AS DATE), 0, 8),
('Execute Test Plan v2.1', 'Complete testing for version 2.1 release', CAST('2026-10-27' AS DATE), 0, 11),
('Bug Report Analysis', 'Categorize and prioritize reported bugs', CAST('2026-11-01' AS DATE), 1, 11),
('Automation Test Suite', 'Build automated test suite for payment module', CAST('2026-11-18' AS DATE), 0, 11),
('Infrastructure Migration', 'Migrate services to new cloud infrastructure', CAST('2026-11-08' AS DATE), 0, 14),
('Monitor CI/CD Pipeline', 'Setup monitoring for deployment pipeline', CAST('2026-11-22' AS DATE), 0, 14);

-- Insert Task Notes
INSERT INTO [dbo].[TaskNotes] ([Content], [CreatedDate], [TaskId]) VALUES
('Started working on Swagger documentation', CAST('2026-10-20' AS DATE), 1),
('Added endpoint descriptions and request/response examples', CAST('2026-10-21' AS DATE), 1),
('Identified issue in JWT token validation', CAST('2026-10-23' AS DATE), 2),
('Bug fixed and tested in staging environment', CAST('2026-10-24' AS DATE), 2),
('Reviewed code structure and best practices', CAST('2026-10-24' AS DATE), 3),
('Approved after minor suggestions', CAST('2026-10-24' AS DATE), 3),
('Planned schema changes with team lead', CAST('2026-10-20' AS DATE), 5),
('Migration script created', CAST('2026-10-21' AS DATE), 5),
('Completed initial competitor analysis', CAST('2026-10-26' AS DATE), 9),
('Report prepared and shared with team', CAST('2026-10-27' AS DATE), 9),
('Drafted new color palette options', CAST('2026-10-25' AS DATE), 11),
('Stakeholders approved final design', CAST('2026-10-30' AS DATE), 11),
('Test cases executed on staging server', CAST('2026-10-26' AS DATE), 13),
('Found 3 critical bugs - reported to development team', CAST('2026-10-27' AS DATE), 13),
('Initial server setup completed', CAST('2026-10-28' AS DATE), 15),
('Database migration in progress', CAST('2026-10-29' AS DATE), 15);

-- ============================================
-- 5. VERIFY DATA
-- ============================================

-- Display summary
SELECT 'Teams' AS [Table], COUNT(*) AS [Count] FROM [dbo].[Team]
UNION ALL
SELECT 'Employees', COUNT(*) FROM [dbo].[Employee]
UNION ALL
SELECT 'TaskItems', COUNT(*) FROM [dbo].[TaskItem]
UNION ALL
SELECT 'TaskNotes', COUNT(*) FROM [dbo].[TaskNotes];