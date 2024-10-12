mergeInto(LibraryManager.library, {
    Shake: function () {
        console.log("TEST");
        const canvas = document.getElementById('unity-canvas');
        if (canvas) {
            let shakeAmount = 20;
            let shakes = 50;
            let shakeInterval = setInterval(() => {
                let x = -50 + (Math.random() * shakeAmount - shakeAmount / 2);  // Default to -50% with shake offset
                let y = Math.random() * shakeAmount - shakeAmount / 2;
                canvas.style.transform = `translate(${x}%, ${y}px)`;
                shakes--;

                if (shakes <= 0) {
                    clearInterval(shakeInterval);
                    canvas.style.transform = 'translate(-50%, 0)';  // Reset to -50% x-axis and 0 y-axis
                }
            }, 50);
        }
    },
});