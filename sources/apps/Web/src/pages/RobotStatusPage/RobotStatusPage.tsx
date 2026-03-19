import styles from './RobotStatusPage.module.css'
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { API_URL } from '../../api/const';
import { TextContainer } from '../../components/TextContainer/TextContainer';
import WarehouseMap from '../../components/WarehouseMap/WarehouseMap';
import type { RobotState } from '../../types/RobotTypes';

const ROBOT_API_URL = API_URL + "api/robot-hub";


export default function RobotStatusPage() {
    const [robotState, setRobotState] = useState<RobotState | null>(null);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(ROBOT_API_URL)
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

    return (
        <div className={styles.container}>
            <div className={styles.leftContainer}>
                <div className={styles.robotStatus}>
                    <p>Status robota</p>
                    <TextContainer title="Akcja" text={robotState?.event}></TextContainer>
                    <TextContainer title="Wykonany ruch" text={robotState?.command}></TextContainer>
                    <TextContainer title="Ostatnia synchronizacja" text={robotState ? new Date(robotState.timestamp * 1000).toLocaleString() : undefined}></TextContainer>
                </div>
            </div>
            <div className={styles.rightContainer}>
                <p>Trasa robota</p>
                <WarehouseMap cols={10} rows={3} stops={5} robotState={robotState}></WarehouseMap>
            </div>
        </div>
    );
}