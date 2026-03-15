using Api.RobotOperations;

namespace Api.RobotService;

public record Commands(List<RobotMoveEnum> commands);