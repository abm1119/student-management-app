// Header scroll shadow
window.addEventListener('scroll', () => {
    document.getElementById('site-header')
        .classList.toggle('scrolled', window.scrollY > 20);
});

// Mobile nav toggle
const navToggle = document.getElementById('nav-toggle');
const navMenu = document.getElementById('nav-menu');
navToggle.addEventListener('click', () => {
    navToggle.classList.toggle('active');
    navMenu.classList.toggle('open');

    // Slide in with GSAP for extra polish
    if (navMenu.classList.contains('open')) {
        gsap.from(navMenu, { x: 300, duration: 0.4, ease: 'power2.out' });
    }
});
