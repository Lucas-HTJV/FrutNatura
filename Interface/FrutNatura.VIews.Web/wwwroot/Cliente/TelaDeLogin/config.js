export function api(path) {
    const API_BASE_URL = "http://localhost:5000/api";
    return `${API_BASE_URL}${path}`;
}
