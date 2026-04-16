import styles from './RobotStatusPage.module.css'
import * as signalR from "@microsoft/signalr";
import { useEffect, useRef, useState } from "react";
import { API_URL } from '../../api/const';
import { TextContainer } from '../../components/TextContainer/TextContainer';
import WarehouseMap from '../../components/WarehouseMap/WarehouseMap';
import type { RobotState } from '../../types/Types';
import { formatDuration } from '../../utils/TimeUtils';
import { WarehouseMapCols, WarehouseMapRows, WarehouseMapStops } from '../../const/const';
import { toPng } from 'html-to-image';

const ROBOT_SIGNALR_API_URL = API_URL + "api/robot-hub";
const ROBOT_STATE_API_URL = API_URL + "api/robot/state";

export default function RobotStatusPage() {
    const [robotState, setRobotState] = useState<RobotState | null>(null);
    const [elapsedTime, setElapsedTime] = useState<number>(0);

    const warehouseMapRef = useRef<HTMLDivElement | null>(null);

    const robotProperties = [
        ["Algorytm", robotState?.order?.tspAlgorithm],
        ["Dystans", robotState?.order?.distance ? `${robotState?.order?.distance.toString()} cm` : undefined],
        ["Akcja", robotState?.event],
        ["Wykonany ruch", robotState?.command?.move],
        ["Ostatnia synchronizacja", robotState?.timestamp ? new Date(robotState.timestamp * 1000).toLocaleString() : undefined],
        ["Czas operacji", robotState?.order?.startPickingTime ? formatDuration(elapsedTime) : undefined],
        ["Poziom zrealizowania zamówienia (%)", undefined]
    ]

    useEffect(() => {
        fetch(ROBOT_STATE_API_URL)
            .then((response) => response.json())
            .then((data: RobotState) => setRobotState(data))
            .catch((error) => console.error("Błąd podczas fetch:", error)); 

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(ROBOT_SIGNALR_API_URL)
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(() => console.log("Connected"))
            .catch(() => console.error("Failed to connect"))

        connection.on("ReceiveRobotState", (message) => {
            try {
                const parsed: RobotState = JSON.parse(message);
                setRobotState(parsed);
                console.log(message);
            } catch (err) {
                console.error("Błąd parsowania", err);
            }
        });

        return () => {
            connection.stop();
        };
    }, []);

    useEffect(() => {
        if (!robotState?.order?.startPickingTime) return;

        const start = robotState.order.startPickingTime;

        const interval = setInterval(() => {
            const now = Date.now();

            if (robotState.order?.finishPickingTime) {
                const end = robotState.order.finishPickingTime;
                setElapsedTime(end - start);
                clearInterval(interval);
            } else {
                setElapsedTime(now - start);
            }
        }, 10);

        return () => clearInterval(interval);
    }, [robotState]);

    const screenshot = async () => {
        if (!warehouseMapRef.current) return;

        const dataUrl = await toPng(warehouseMapRef.current, {backgroundColor:"white"});
        const link = document.createElement("a");
        link.download = `order-robot-state_${new Date().toISOString().replace(/[:.]/g, "-")}.png`;
        link.href = dataUrl;
        link.click();
    };

    return (
        <div className={styles.container}>
            <div className={styles.leftContainer}>
                <div className={styles.robotStatus}>
                    <h3>Status robota</h3>
                    <table className={styles.table}>
                        <thead>
                            <tr>
                                <th>Właściwość</th>
                                <th>Wartość</th>
                            </tr>
                        </thead>
                        <tbody>
                            {robotProperties.map((robotProperty) => {
                                const [label, value] = robotProperty;

                                return (
                                    <tr>
                                        <td>{label}</td>
                                        <td><TextContainer text={value}></TextContainer></td>
                                    </tr>
                                )
                            })}
                        </tbody>
                    </table>
                </div>
            </div>
            <div className={styles.rightContainer}>
                <h3>Trasa robota</h3>
                <div ref={warehouseMapRef} className={styles.warehouseMapContainer}>
                    <WarehouseMap cols={WarehouseMapCols} rows={WarehouseMapRows} stops={WarehouseMapStops} robotState={robotState}></WarehouseMap>
                </div>
                <div>
                    <button className={styles.screenshot} onClick={() => screenshot()}>Pobierz stan mapy</button>
                </div>
            </div>
        </div>
    );
}