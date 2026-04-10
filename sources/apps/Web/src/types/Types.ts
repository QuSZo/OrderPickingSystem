export interface RobotState {
    position: Position;
    direction: Direction;
    event: Event;
    timestamp: number;
    command?: RobotCommand;
    order?: Order;
    productBeingPicked?: OrderedProduct
}

export interface Position {
    x: number;
    y: number;
} 

export interface RobotCommand {
    move: RobotMove;
    stopDurationMs: number | undefined
    orderedProduct: OrderedProduct | undefined
}

export type RobotMove = "forward" | "back" | "left" | "right" | "stop";

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
    pickedProducts: OrderedProduct[];
    tspAlgorithm: TspAlgorithms;
    timestamp: number;
    distance: number;
    startPickingTime: number;
    finishPickingTime: number | null;
}

export interface OrderedProduct {
    id: string;
    name: string;
    position: Position;
    quantity: number;
}

export type TspAlgorithms = "DijkstraZachłanny" | "AStarZachłanny" | "Networkx" | "Naiwny" | "BrutalnaSiła";

export const algorithms: TspAlgorithms[] = ["DijkstraZachłanny", "AStarZachłanny", "Networkx", "Naiwny", "BrutalnaSiła"];

export interface AverageDuration{
    algorithm: TspAlgorithms;
    averageDurationMs: number;
}

export interface AverageDurationByOrderSize{
    algorithm: TspAlgorithms;
    productCount: number;
    averageDurationMs: number;
}