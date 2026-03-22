import styles from './RobotStatusPage.module.css'
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { API_URL } from '../../api/const';
import { TextContainer } from '../../components/TextContainer/TextContainer';
import WarehouseMap from '../../components/WarehouseMap/WarehouseMap';
import type { RobotState } from '../../types/Types';
import { formatDuration } from '../../utils/TimeUtils';

const ROBOT_SIGNALR_API_URL = API_URL + "api/robot-hub";
const ROBOT_STATE_API_URL = API_URL + "api/robot/state";

export default function RobotStatusPage() {
    const [robotState, setRobotState] = useState<RobotState | null>(null);
    const [elapsedTime, setElapsedTime] = useState<number>(0);

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

    return (
        <div className={styles.container}>
            <div className={styles.leftContainer}>
                <div className={styles.robotStatus}>
                    <h3>Status robota</h3>
                    <TextContainer title="Akcja" text={robotState?.event}></TextContainer>
                    <TextContainer title="Wykonany ruch" text={robotState?.command}></TextContainer>
                    <TextContainer title="Ostatnia synchronizacja" text={robotState?.timestamp ? new Date(robotState.timestamp * 1000).toLocaleString() : undefined}></TextContainer>
                    <TextContainer title="Czas operacji" text={robotState?.order?.startPickingTime ? formatDuration(elapsedTime) : undefined} />
                    <TextContainer title="Przejechana odległość" text={undefined} />
                </div>
            </div>
            <div className={styles.rightContainer}>
                <h3>Trasa robota</h3>
                <WarehouseMap cols={3} rows={3} stops={5} robotState={robotState}></WarehouseMap>
            </div>
        </div>
    );
}