using System.Runtime.CompilerServices;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Logs;

public static partial class LogsExtension
{
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Information,
		Message = "New user {email} registered")]
	public static partial void LogUserCreated(this ILogger logger, string email);
	
	[LoggerMessage(
		EventId = 2,
		Level = LogLevel.Information,
		Message = "New building {buildingCode} created with id {id}")]
	public static partial void LogBuildingCreated(this ILogger logger, string buildingCode, int id);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogBuildingCreated(this ILogger logger, Building building) => LogBuildingCreated(logger, building.Code, building.Id);
	
	[LoggerMessage(
		EventId = 3,
		Level = LogLevel.Information,
		Message = "Building {buildingCode} deleted")]
	public static partial void LogBuildingDeleted(this ILogger logger, string buildingCode);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogBuildingDeleted(this ILogger logger, Building building) => LogBuildingDeleted(logger, building.Code);
	
	[LoggerMessage(
		EventId = 4,
		Level = LogLevel.Information,
		Message = "New classroom {classroomCode} created in building {buildingCode} with id {classroomId}")]
	public static partial void LogClassroomCreated(this ILogger logger, string classroomCode, string buildingCode, int classroomId);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogClassroomCreated(this ILogger logger, Classroom classroom) => LogClassroomCreated(logger, classroom.Code, classroom.Building.Code, classroom.Id);

	[LoggerMessage(
		EventId = 5,
		Level = LogLevel.Information,
		Message = "Classroom {classroomCode} deleted in building {buildingCode}")]
	public static partial void LogClassroomDeleted(this ILogger logger, string classroomCode, string buildingCode);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogClassroomDeleted(this ILogger logger, Classroom classroom) => LogClassroomDeleted(logger, classroom.Code, classroom.Building.Code);
}