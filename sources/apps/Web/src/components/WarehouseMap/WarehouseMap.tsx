import { useState, useEffect } from "react";
import type { RobotEvent } from "../../types/RobotTypes";


type Direction = "N" | "E" | "S" | "W";

interface WarehouseMapProps {
  rows: number;
  cols: number;
  robotEvent: RobotEvent | null;
}

const CELL_SIZE = 60;

export default function WarehouseMap({
  rows,
  cols,
  robotEvent
}: WarehouseMapProps) {
  const [position, setPosition] = useState({ x: 0, y: 0 });
  const [direction, setDirection] = useState<Direction>("E");

  useEffect(() => {
    if (!robotEvent || robotEvent.event !== "movement") return;
    if (!robotEvent.command) return;

    setPosition(prev => {
      let { x, y } = prev;
      let newDirection = direction;

      switch (robotEvent.command) {
        case "left":
          newDirection =
            direction === "N" ? "W" :
            direction === "W" ? "S" :
            direction === "S" ? "E" : "N";
          setDirection(newDirection);
          return prev;

        case "right":
          newDirection =
            direction === "N" ? "E" :
            direction === "E" ? "S" :
            direction === "S" ? "W" : "N";
          setDirection(newDirection);
          return prev;

        case "forward":
          if (direction === "N") y -= 1;
          if (direction === "S") y += 1;
          if (direction === "E") x += 1;
          if (direction === "W") x -= 1;
          break;

        case "back":
          if (direction === "N") y += 1;
          if (direction === "S") y -= 1;
          if (direction === "E") x -= 1;
          if (direction === "W") x += 1;
          break;
      }

      // ograniczenie do granic magazynu
      x = Math.max(0, Math.min(cols - 1, x));
      y = Math.max(0, Math.min(rows - 1, y));

      return { x, y };
    });

  }, [robotEvent]);

  const width = cols * CELL_SIZE;
  const height = rows * CELL_SIZE;

  return (
    <svg width={width} height={height} style={{ border: "1px solid black" }}>
      
      {/* Linie pionowe */}
      {Array.from({ length: cols }).map((_, i) => (
        <line
          key={`v-${i}`}
          x1={i * CELL_SIZE}
          y1={0}
          x2={i * CELL_SIZE}
          y2={height}
          stroke="gray"
        />
      ))}

      {/* Linie poziome */}
      {Array.from({ length: rows }).map((_, i) => (
        <line
          key={`h-${i}`}
          x1={0}
          y1={i * CELL_SIZE}
          x2={width}
          y2={i * CELL_SIZE}
          stroke="gray"
        />
      ))}

      {/* Robot */}
      <circle
        cx={position.x * CELL_SIZE}
        cy={position.y * CELL_SIZE}
        r={10}
        fill="red"
      />
    </svg>
  );
}