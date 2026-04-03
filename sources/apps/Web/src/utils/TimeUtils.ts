export const formatDuration = (ms: number) => {
    const h = Math.floor(ms / 3600000);
    ms %= 3600000;

    const m = Math.floor(ms / 60000);
    ms %= 60000;

    const s = Math.floor(ms / 1000);

    const parts = [];

    if (h > 0) {
        parts.push(`${h} h`, `${m} min`, `${s} s`);
    } 
    else if (m > 0) {
        parts.push(`${m} min`, `${s} s`);
    }
    else {
        parts.push(`${s} s`);
    }

    return parts.join(' ');
}