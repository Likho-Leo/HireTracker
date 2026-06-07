
function formatVehicles(vehicleString) {
    if (!vehicleString) return '';

    const vehicles = vehicleString.split(', ');
    const counts = {};

    vehicles.forEach(v => {
        counts[v] = (counts[v] || 0) + 1;
    });

    return Object.entries(counts)
        .map(([vehicle, count]) => count > 1 ? `${vehicle} x${count}` : vehicle)
        .join(', ');
}