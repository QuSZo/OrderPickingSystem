export type RobotCommand = "forward" | "back" | "left" | "right";

export interface RobotEvent {
    event: "movement" | "finished" | "stopped";
    timestamp: number;
    command?: string;
}