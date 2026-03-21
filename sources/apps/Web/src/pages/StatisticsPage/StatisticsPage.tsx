import { useEffect, useState } from 'react';
import { API_URL } from '../../api/const';
import styles from './StatisticsPage.module.css'
import { BarChart, Bar, XAxis, YAxis, Tooltip, CartesianGrid, Label, LineChart, Legend, Line } from "recharts";
import { formatDuration } from '../../utils/TimeUtils';
import type { AverageDuration, AverageDurationByOrderSize } from '../../types/Types';

const STATISTICS_API_URL = API_URL + "api/statistics";

function transformData(data: AverageDurationByOrderSize[]) {
    const map: Record<number, any> = {};

    data.forEach(item => {
        if (!map[item.productCount]) {
        map[item.productCount] = { productCount: item.productCount };
        }

        map[item.productCount][item.algorithm] = item.averageDurationMs;
    });

    return Object.values(map);
}

export default function StatisticsPage() {
    const [averageDuration, setAverageDuration] = useState<AverageDuration[]>([]);
    const [averageDurationByOrderSize, setAverageDurationByOrderSize] = useState<AverageDurationByOrderSize[]>([]);

    useEffect(() => {
        fetch(`${STATISTICS_API_URL}/average-duration-by-algorithm`)
            .then(res => res.json())
            .then((data: AverageDuration[]) => setAverageDuration(data));

        fetch(`${STATISTICS_API_URL}/average-duration-by-algorithm-and-order-size`)
            .then(res => res.json())
            .then((data: AverageDurationByOrderSize[]) => setAverageDurationByOrderSize(data));
    }, []);

    return (
        <div className={styles.container}>
            <BarChart width={600} height={300} data={averageDuration}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="algorithm" />
                <YAxis tickFormatter={(value) => formatDuration(value)} tick={{ fontSize: 12 }} >
                    <Label value="Czas" angle={-90} position="insideLeft" style={{ textAnchor: "middle" }} />
                </YAxis>
                <Tooltip formatter={(value) => formatDuration(value as number)} />
                <Bar dataKey="averageDurationMs" fill="#dadada" />
            </BarChart>
            <LineChart width={600} height={300} data={transformData(averageDurationByOrderSize)}>
                <CartesianGrid strokeDasharray="3 3" opacity={0.2} />
                <XAxis dataKey="productCount" />
                <YAxis tickFormatter={(value) => formatDuration(value)} />
                <Tooltip formatter={(value) => formatDuration(value as number)} />
                <Legend />
                <Line type="monotone" dataKey="Naive" stroke="#8884d8" />
            </LineChart>
        </div>
    );
}

