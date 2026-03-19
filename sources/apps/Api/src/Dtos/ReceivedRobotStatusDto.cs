using Api.RobotOperations;

namespace Api.Dtos;

public record ReceivedRobotStatusDto(string Event, RobotMoveEnum? Command, double Timestamp);