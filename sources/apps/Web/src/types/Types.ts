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

export interface OrderDto {
    orderedProducts: OrderedProduct[];
    tspAlgorithm: TspAlgorithms;
    timestamp: number;
}

export interface Order {
    orderId: string;
    orderedProducts: OrderedProduct[];
    tspAlgorithm: TspAlgorithms;
    timestamp: number;
    startPickingTime: number;
    finishPickingTime: number | null;
}

export interface OrderedProduct {
    id: string;
    name: string;
    position: Position;
    quantity: number;
}

export type TspAlgorithms = "Naive";

export const algorithms: TspAlgorithms[] = ["Naive"];