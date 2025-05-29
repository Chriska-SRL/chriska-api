CREATE TABLE [dbo].[ReturnRequests]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Date] DATETIME NOT NULL, 
    [Observations] NVARCHAR(255) NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL,
    [ClientId] INT NOT NULL, 
    [CreditNoteId] INT NULL,
    [UserId] INT NOT NULL, 
    CONSTRAINT [FK_ReturnRequests_Clients] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id]),
    CONSTRAINT [FK_ReturnRequests_CreditNotes] FOREIGN KEY ([CreditNoteId]) REFERENCES [CreditNotes]([Id]),
    CONSTRAINT [FK_ReturnRequests_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
