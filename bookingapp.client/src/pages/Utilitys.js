export function formatDateTime(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0'); // dd
    const month = String(date.getMonth() + 1).padStart(2, '0'); // MM
    const year = date.getFullYear(); // yyyy
    const hours = String(date.getHours()).padStart(2, '0'); // HH
    const minutes = String(date.getMinutes()).padStart(2, '0'); // mm
    const seconds = String(date.getSeconds()).padStart(2, '0'); // ss
  
    return `${day}/${month}/${year} : ${hours}:${minutes}:${seconds}`;
  }

export function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0'); // dd
    const month = String(date.getMonth() + 1).padStart(2, '0'); // MM
    const year = date.getFullYear(); // yyyy
    const hours = String(date.getHours()).padStart(2, '0'); // HH
    const minutes = String(date.getMinutes()).padStart(2, '0'); // mm
    const seconds = String(date.getSeconds()).padStart(2, '0'); // ss
  
    return `${day}/${month}/${year}`;
  }
  
 export function formatNumber(number, decimals = 2) {
    if (isNaN(number)) return "Invalid number";
  
    return number
      .toFixed(decimals)
      .replace(/\B(?=(\d{3})+(?!\d))/g, ","); 
  }