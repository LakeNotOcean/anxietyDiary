export class ButtonState {
  #isActive: boolean;
  #content: string;
  constructor(isActive: boolean = false, content = "Открыть") {
    this.#isActive = isActive;
    this.#content = content;
  }
  get isActive() {
    return this.#isActive;
  }
  set isActive(isActive: boolean) {
    this.#isActive = isActive;
  }
  get content() {
    return this.#content;
  }
  set content(content: string) {
    this.#content = content;
  }

  public clone(): ButtonState {
    return new ButtonState(this.#isActive, this.#content);
  }
}
