import { useState, useEffect, useRef } from "react";
import type { Direction, Position, RobotState } from "../../types/Types";

interface WarehouseMapProps {
  rows: number;
  cols: number;
  stops: number;
  robotState: RobotState | null;
}

const CELL_SIZE_X = 300;
const CELL_SIZE_Y = 300;

const getDirectionOffset = (direction: Direction) => {
  const offset = 15;

  switch (direction) {
    case "north":
      return { dx: 0, dy: -offset };
    case "south":
      return { dx: 0, dy: offset };
    case "east":
      return { dx: offset, dy: 0 };
    case "west":
      return { dx: -offset, dy: 0 };
  }
};

export default function WarehouseMap({ rows, cols, stops, robotState }: WarehouseMapProps) {
  const [visitedPositions, setVisitedPositions] = useState<Position[]>([]);

  const robotRef = useRef<SVGCircleElement | null>(null);
  const directionRef = useRef<SVGCircleElement | null>(null);

  useEffect(() => {
    if (!robotState) return;

    setVisitedPositions(prev => {
      const last = prev[prev.length - 1];
      if (!last || last.x !== robotState.currentPosition.x || last.y !== robotState.currentPosition.y) {
        return [...prev, robotState.currentPosition];
      }
      return prev;
    });

    const start = robotState.currentPosition;
    const end = robotState.nextPosition ?? robotState.currentPosition;

    const duration = 5000;
    let startTime: number | null = null;
    let animationFrameId: number;

    const animate = (time: number) => {
      if (!startTime) {
        startTime = time;
      }

      const linear = (time - startTime) / duration;
      const progress = Math.min(1, 1 - Math.pow(1 - linear, 3));

      const newPosition = {
        x: start.x + (end.x - start.x) * progress,
        y: start.y + (end.y - start.y) * progress,
      };

      const robotX = newPosition.x * CELL_SIZE_X;
      const robotY = newPosition.y * CELL_SIZE_Y / 6;

      if (robotRef.current) {
        robotRef.current.setAttribute("cx", String(robotX));
        robotRef.current.setAttribute("cy", String(robotY));
      }

      const { dx, dy } = getDirectionOffset(robotState.direction);

      if (directionRef.current) {
        directionRef.current.setAttribute("cx", String(robotX + dx));
        directionRef.current.setAttribute("cy", String(robotY + dy));
      }

      if (progress < 1) {
        animationFrameId = requestAnimationFrame(animate);
      }
    };

    animationFrameId = requestAnimationFrame(animate);

    return () => cancelAnimationFrame(animationFrameId);

  }, [robotState]);

  const width = (cols - 1) * CELL_SIZE_X;
  const height = (rows - 1) * CELL_SIZE_Y;

  const stopCount = (rows - 1) * stops + rows;
  
  return (
    <svg width={width} height={height} style={{ overflow: "visible" }}>
      
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
      
      {Array.from({ length: cols }).map((_, colNumber) => (
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

      {robotState?.order?.orderedProducts.map((orderedProduct) => (
        <g>
          <circle cx={orderedProduct.position.x*CELL_SIZE_X} cy={orderedProduct.position.y*CELL_SIZE_Y/6} r={10} fill="orange" />
          <text fontWeight="bold" x={orderedProduct.position.x*CELL_SIZE_X + 20} y={orderedProduct.position.y*CELL_SIZE_Y/6 + 5}>{orderedProduct.name}</text>
        </g>
      ))}

      {visitedPositions.slice(1).map((position, i) => {
        const prevPosition = visitedPositions[i];
        return (
        <line
          key={`path-${i}`}
          x1={prevPosition.x * CELL_SIZE_X}
          y1={prevPosition.y * CELL_SIZE_Y / 6}
          x2={position.x * CELL_SIZE_X}
          y2={position.y * CELL_SIZE_Y / 6}
          stroke="green"
        />
      )})}

      {robotState?.order?.pickedProducts.map((pickedProduct) => (
        <g>
          <circle cx={pickedProduct.position.x*CELL_SIZE_X} cy={pickedProduct.position.y*CELL_SIZE_Y/6} r={10} fill="green" />
        </g>
      ))}

      <circle ref={robotRef} cx={0} cy={0} r={18} fill="red" />
      <circle ref={directionRef} cx={0} cy={0} r={6} fill="black" />
    </svg>
  );
}