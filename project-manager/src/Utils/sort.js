export const mapOrder = (array, order, key) => {
    array.sort((a,b) => order.indexOf(a[key]) - order.indexOf(b[key])); 
    return array;
}


export const mapOrderCol = (array) => {
    array.sort((a,b) => a.order - b.order)
    return array;
}