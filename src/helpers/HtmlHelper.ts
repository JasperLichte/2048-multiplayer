export class HtmlHelper {

  private static element(type: string) {
    return function(content: string|HTMLElement, attribs: {} = {}) {
      return function(target: HTMLElement): HTMLElement {
        const element = document.createElement(type);

        for (const attrib in attribs) {
          element.setAttribute(attrib, attribs[attrib]);
        }

        if (typeof content === 'string') {
          element.innerText = content;
        } else {
          element.appendChild(content);
        }
        target.appendChild(element);
        return element;
      }
    }
  }

  public static div(
    content: string|HTMLElement,
    attribs: {} = {},
    target: HTMLElement
  ) {
    return HtmlHelper.element('div')(content, attribs)(target);
  }

  public static span(
    content: string|HTMLElement,
    attribs: {} = {},
    target: HTMLElement
  ) {
    return HtmlHelper.element('span')(content, attribs)(target);
  }

  public static h1(
    content: string|HTMLElement,
    attribs: {} = {},
    target: HTMLElement
  ) {
    return HtmlHelper.element('h1')(content, attribs)(target);
  }

  public static h2(
    content: string|HTMLElement,
    attribs: {} = {},
    target: HTMLElement
  ) {
    return HtmlHelper.element('h2')(content, attribs)(target);
  }

  public static p(
    content: string|HTMLElement,
    attribs: {} = {},
    target: HTMLElement
  ) {
    return HtmlHelper.element('p')(content, attribs)(target);
  }

}
