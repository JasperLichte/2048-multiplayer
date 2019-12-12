export default class DomHelper {
}

export const $ = (selector: string): HTMLElement => document.querySelector(selector);
export const $$ = (selector: string): NodeListOf<HTMLElement> => document.querySelectorAll(selector);
