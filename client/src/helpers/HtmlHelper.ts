export default class HtmlHelper {

  private static element(type: string) {
    return function(content: string|HTMLElement|HTMLElement[] = null, attribs: {} = {}) {
      return function(target: HTMLElement = null): HTMLElement {
        const element = document.createElement(type);

        for (const attrib in attribs) {
          element.setAttribute(attrib, attribs[attrib]);
        }

        if (content) {
          if (typeof content === 'string') {
            element.innerText = content;
          } else if (Array.isArray(content)) {
            content.forEach(c => element.appendChild(c));
          } else {
            element.appendChild(content);
          }
        }

        if (target) {
          target.appendChild(element);
        }

        return element;
      }
    }
  }

  public static div(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('div')(content, attribs)(target);
  }

  public static span(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('span')(content, attribs)(target);
  }

  public static h1(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('h1')(content, attribs)(target);
  }

  public static h2(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('h2')(content, attribs)(target);
  }

  public static h3(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('h3')(content, attribs)(target);
  }

  public static p(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('p')(content, attribs)(target);
  }

  public static table(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('table')(content, attribs)(target);
  }

  public static tr(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('tr')(content, attribs)(target);
  }

  public static td(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('td')(content, attribs)(target);
  }

  public static th(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('th')(content, attribs)(target);
  }

  public static ul(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('ul')(content, attribs)(target);
  }

  public static li(
    content: string|HTMLElement|HTMLElement[] = null,
    attribs: {} = {},
    target: HTMLElement = null
  ): HTMLElement {
    return HtmlHelper.element('li')(content, attribs)(target);
  }

}
