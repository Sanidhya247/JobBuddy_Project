import React, { useState } from 'react';
import '../assets/css/AccessibilityPanel.css';

function AccessibilityPanel() {
    const [isPanelVisible, setIsPanelVisible] = useState(false);

    const togglePanelVisibility = () => {
        setIsPanelVisible(!isPanelVisible);
    };

    const toggleGoogleTranslate = () => {
        const translateElement = document.getElementById('google_translate_element');
        if (translateElement) {
            translateElement.style.display = 'block';
        }
    };

    const adjustFontSize = (size) => {
        document.querySelectorAll('body, body *').forEach((element) => {
            if (!element.style.fontSize) {
                element.style.fontSize = '1em';
            }
            const currentSize = parseFloat(element.style.fontSize.replace('em', ''));
            element.style.fontSize = size === 'increase' ? `${currentSize + 0.1}em` : `${Math.max(1, currentSize - 0.1)}em`;
        });
    };

    const toggleHighContrast = () => {
        document.body.classList.toggle('high-contrast');
    };

    const toggleGrayscaleMode = () => {
        document.body.classList.toggle('grayscale');
    };

    const toggleDyslexiaFont = () => {
        document.body.classList.toggle('dyslexia-font');
    };

    const readPageContent = () => {
        const text = document.body.innerText;
        const speech = new SpeechSynthesisUtterance(text);
        window.speechSynthesis.speak(speech);
    };

    return (
        <>
            <div
                className="accessibility-dock"
                onClick={togglePanelVisibility}
                onMouseEnter={() => setIsPanelVisible(true)}
                onMouseLeave={() => setIsPanelVisible(false)}
            >
                <span role="img" aria-label="Accessibility Tools">♿</span>
            </div>
            <div
                className={`accessibility-slider ${isPanelVisible ? 'show' : ''}`}
                onMouseEnter={() => setIsPanelVisible(true)}
                onMouseLeave={() => setIsPanelVisible(false)}
            >
                <button className="accessibility-button" onClick={toggleGoogleTranslate}>
                    Translate to French/English
                </button>
                <button className="accessibility-button" onClick={() => adjustFontSize('increase')}>
                    Increase Font Size
                </button>
                <button className="accessibility-button" onClick={() => adjustFontSize('decrease')}>
                    Decrease Font Size
                </button>
                <button className="accessibility-button" onClick={toggleHighContrast}>
                    Toggle High Contrast
                </button>
                <button className="accessibility-button" onClick={toggleGrayscaleMode}>
                    Toggle Grayscale Mode
                </button>
                <button className="accessibility-button" onClick={toggleDyslexiaFont}>
                    Dyslexia-Friendly Font
                </button>
                <button className="accessibility-button" onClick={readPageContent}>
                    Read Page Content
                </button>
            </div>
        </>
    );
}

export default AccessibilityPanel;
