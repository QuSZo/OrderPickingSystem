using Api.RobotOperations;

namespace Api.Dtos;

public record RobotCommandDto(List<RobotMoveEnum> commands);