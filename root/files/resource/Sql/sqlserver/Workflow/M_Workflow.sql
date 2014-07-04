USE [Workflow]
GO

/****** Object:  Table [dbo].[M_Workflow]    Script Date: 2014/07/03 14:08:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[M_Workflow](
	[Id] [decimal](10, 0) NOT NULL,
	[SubSystemId] [char](4) NOT NULL,
	[WorkflowName] [varchar](50) NOT NULL,
	[WfPositionId] [int] NOT NULL,
	[FromUserId] [decimal](10, 0) NULL,
	[ActionType] [varchar](30) NOT NULL,
	[ToUserId] [decimal](10, 0) NULL,
	[ToUserPositionTitlesId] [decimal](10, 0) NULL,
	[SortIndex] [int] NULL,
	[NextWfPositionId] [int] NULL,
	[MailTemplateId] [decimal](10, 0) NULL,
	[ReserveArea] [varchar](50) NULL,
 CONSTRAINT [PK_M_Workflow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [Workflow]
GO

/****** Object:  Index [IX_M_Workflow]    Script Date: 2014/07/03 14:08:46 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_M_Workflow] ON [dbo].[M_Workflow]
(
	[SubSystemId] ASC,
	[WorkflowName] ASC,
	[WfPositionId] ASC,
	[FromUserId] ASC,
	[ActionType] ASC,
	[ToUserId] ASC,
	[SortIndex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


