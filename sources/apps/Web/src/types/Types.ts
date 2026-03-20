export interface RobotState {
    position: Position;
    direction: Direction;
    event: Event;
    timestamp: number;
    command?: RobotCommand;
}

export interface Position {
    x: number;
    y: number;
} 

export type RobotCommand = "forward" | "back" | "left" | "right";

export type Direction = "north" | "east" | "south" | "west";

export type Event = "movement" | "finished" | "stopped";

export interface Product {
    id: string;
    name: string;
    position: Position;
};

export interface OrderedProduct {
    id: string;
    name: string;
    position: Position;
    quantity: number;
}