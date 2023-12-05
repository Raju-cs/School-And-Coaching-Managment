import { READ_URL } from "./site.js";

export const url = (endpoint) => {
    return endpoint ? `${endpoint}` : null;
}

export function dateBound(el, date) {
    if (!date) el.html('');
    el.html(new Date(date).toLocaleString('en-US'));
}


export function imageBound(td) {
    td.html(`<img src="${url(this.ImageURL)}" style="max-height: 120px; max-width: 100%;" />`);
}


export function unApproveimageBound(td) {
    td.html(`<img src="/images/student/icon/${url(this.ImageURL)}" style="max-height: 120px; max-width: 100%;" />`);
}
