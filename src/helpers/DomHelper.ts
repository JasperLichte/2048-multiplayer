export default class DomHelper {
}

export const $ = (selector: string): HTMLElement => document.querySelector(selector);
export const $$ = (selector: string): NodeListOf<HTMLElement> => document.querySelectorAll(selector);

export const initProto = () => {
  // @ts-ignore
  HTMLElement.prototype.$ = function(selector: string): HTMLElement {
    return this.querySelector(selector);
  };
  // @ts-ignore
  HTMLElement.prototype.$$ = function(selector: string): NodeListOf<HTMLElement> {
    return this.querySelectorAll(selector);
  };
};
