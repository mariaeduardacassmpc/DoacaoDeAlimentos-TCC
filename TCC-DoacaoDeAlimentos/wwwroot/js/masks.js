export function applyMask(inputId, maskPattern, dotNetHelper) {
    const element = document.getElementById(inputId);
    if (!element) return;

    const mask = IMask(element, {
        mask: maskPattern
    });

    element.addEventListener('input', () => {
        dotNetHelper.invokeMethodAsync('UpdateFromJS', mask.value);
    });

    return {
        destroy: () => {
            mask.destroy();
            element.removeEventListener('input', () => { });
        }
    };
}