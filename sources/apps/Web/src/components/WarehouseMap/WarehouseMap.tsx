import { useState, useEffect } from "react";
import type { RobotEvent } from "../../types/RobotTypes";


type Direction = "N" | "E" | "S" | "W";

interface WarehouseMapProps {
  rows: number;
  cols: number;
  stops: number;
  robotEvent: RobotEvent | null;
}

const CELL_SIZE_X = 60;
const CELL_SIZE_Y = 180;

const calculateDirection = (command: string, direction: Direction) => {
  let newDirection : Direction = "N";

  switch (command) {
    case "forward":
      newDirection = direction;
      break;
    case "left":
      newDirection = direction === "N" ? "W" : direction === "W" ? "S" : direction === "S" ? "E" : "N";
      break;
    case "right":
      newDirection = direction === "N" ? "E" : direction === "E" ? "S" : direction === "S" ? "W" : "N";
      break;
    case "back":
      newDirection = direction === "N" ? "S" : direction === "W" ? "E" : direction === "S" ? "N" : "W";
      break;
    default:
      newDirection = "N";
      break;
  }

  return newDirection;
}

const getDirectionOffset = (direction: Direction) => {
  const offset = 15;

  switch (direction) {
    case "N":
      return { dx: 0, dy: -offset };
    case "S":
      return { dx: 0, dy: offset };
    case "E":
      return { dx: offset, dy: 0 };
    case "W":
      return { dx: -offset, dy: 0 };
  }
};

export default function WarehouseMap({ rows, cols, stops, robotEvent }: WarehouseMapProps) {
  const [position, setPosition] = useState({ x: 0, y: 0 });
  const [direction, setDirection] = useState<Direction>("S");

  useEffect(() => {
    if (!robotEvent || robotEvent.event !== "movement") return;
    if (!robotEvent.command) return;

    setPosition(prev => {
      let { x, y } = prev;

      switch (robotEvent.command) {
        case "forward":
          if (direction === "N") y -= 1;
          if (direction === "S") y += 1;
          if (direction === "E") x += 1;
          if (direction === "W") x -= 1;
          break;

        case "left":
          if (direction === "N") x -= 1;
          if (direction === "S") x += 1;
          if (direction === "E") y -= 1;
          if (direction === "W") y += 1;
          break;

        case "right":
          if (direction === "N") x += 1;
          if (direction === "S") x -= 1;
          if (direction === "E") y += 1;
          if (direction === "W") y -= 1;
          break;

        case "back":
          if (direction === "N") y += 1;
          if (direction === "S") y -= 1;
          if (direction === "E") x -= 1;
          if (direction === "W") x += 1;
          break;
      }

      setDirection(calculateDirection(robotEvent.command ?? "", direction))

      x = Math.max(0, Math.min(cols, x));
      y = Math.max(0, Math.min(rows * stops + rows + 1 - 1, y));

      return { x, y };
    });

  }, [robotEvent]);

  const width = cols * CELL_SIZE_X;
  const height = rows * CELL_SIZE_Y;

  const robotX = position.x * CELL_SIZE_X;
  const robotY = position.y * CELL_SIZE_Y/6;

  const stopCount = rows * stops + rows+1;
  
  return (
    <svg width={width} height={height} style={{ border: "1px solid black", overflow: "visible" }}>
      
      {Array.from({ length: cols }).map((_, i) => (
        <line
          key={`v-${i}`}
          x1={i * CELL_SIZE_X}
          y1={0}
          x2={i * CELL_SIZE_X}
          y2={height}
          stroke="gray"
        />
      ))}

      {Array.from({ length: rows }).map((_, i) => (
        <line
          key={`h-${i}`}
          x1={0}
          y1={i * CELL_SIZE_Y}
          x2={width}
          y2={i * CELL_SIZE_Y}
          stroke="gray"
        />
      ))}

      
      {Array.from({ length: cols+1 }).map((_, colNumber) => (
        Array.from({ length: stopCount }).map((_, stopNumber) => (
          <line
            key={`stop-${colNumber}-${stopNumber}`}
            x1={-20 + colNumber * CELL_SIZE_X}
            y1={stopNumber * CELL_SIZE_Y / (stops+1)}
            x2={20 + colNumber * CELL_SIZE_X}
            y2={stopNumber * CELL_SIZE_Y / (stops+1)}
            stroke="gray"
          />
        ))
      ))}

      <circle cx={robotX} cy={robotY} r={18} fill="red" />

      {(() => {
        const { dx, dy } = getDirectionOffset(direction);
        return (
        <circle cx={robotX + dx} cy={robotY + dy} r={6} fill="black"/>
        );
      })()}
    </svg>
  );
}