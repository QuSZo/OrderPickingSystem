export const formatDuration = (ms: number) => {
    const h = Math.floor(ms / 3600000);
    ms %= 3600000;

    const m = Math.floor(ms / 60000);
    ms %= 60000;

    const s = Math.floor(ms / 1000);
    ms %= 1000;

    const parts = [];

    if (h > 0) parts.push(`${h}h`);
    if (m > 0) parts.push(`${m}m`);
    if (s > 0) parts.push(`${s}s`);
    if (ms > 0 || parts.length === 0) parts.push(`${ms}ms`);

    return parts.join(' ');
}