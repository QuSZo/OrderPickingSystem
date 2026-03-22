import { useEffect, useState } from 'react';
import { API_URL } from '../../api/const';
import styles from './StatisticsPage.module.css'
import { BarChart, Bar, XAxis, YAxis, Tooltip, CartesianGrid, LineChart, Legend, Line } from "recharts";
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
            <div className={styles.chartContainer}>
                <h4>Średni czas trwania zbierania w zależności od algorytmu</h4>
                <BarChart width={600} height={300} data={averageDuration}>
                    <CartesianGrid strokeDasharray="3 3" opacity={0.5} />
                    <XAxis dataKey="algorithm" />
                    <YAxis tickFormatter={(value) => formatDuration(value)} tick={{ fontSize: 12 }} />
                    <Tooltip formatter={(value) => formatDuration(value as number)} />
                    <Bar dataKey="averageDurationMs" fill="#dadada" />
                </BarChart>
            </div>
            <div className={styles.chartContainer}>
                <h4>Średni czas trwania zbierania w zależności od algorytmu i liczby produktów</h4>
                <LineChart width={600} height={300} data={transformData(averageDurationByOrderSize)}>
                    <CartesianGrid strokeDasharray="3 3" opacity={0.5} />
                    <XAxis dataKey="productCount" />
                    <YAxis tickFormatter={(value) => formatDuration(value)} tick={{ fontSize: 12 }} />
                    <Tooltip formatter={(value) => formatDuration(value as number)} />
                    <Legend layout="vertical" align="right" verticalAlign="middle" />
                    <Line type="monotone" dataKey="Naive" stroke="#8884d8" />
                    <Line type="monotone" dataKey="BruteForce" stroke="#82ca9d" />
                </LineChart>
            </div>
        </div>
    );
}

