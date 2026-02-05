import { Link, useLocation } from 'react-router-dom';
import { useState } from 'react';
import NearestSection from './Nearest/NearestSection';

export default function Header() {
    const location = useLocation();
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const isActive = (path) => {
        return location.pathname === path;
    };

    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
    };

    return (
        <nav className="navbar navbar-expand-md navbar-dark fixed-top bg-dark border-bottom border-secondary">
            <div className="container-fluid">
                <img
                    style={{
                        height: "50px",
                        paddingRight: "15px"
                    }}
                    src="miet.ico"
                    alt="MIET Logo"
                />
                <a className="navbar-brand me-4">MIET Schedule</a>

                <button
                    className="navbar-toggler ms-3"
                    type="button"
                    onClick={toggleMenu}
                    aria-controls="navbarCollapse"
                    aria-expanded={isMenuOpen}
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>

                <div className={`collapse navbar-collapse ${isMenuOpen ? 'show' : ''}`} id="navbarCollapse">
                    <ul className="navbar-nav me-auto mb-2 mb-md-0">
                        <li className="nav-item">
                            <Link
                                to="/"
                                className={`nav-link ${isActive('/') ? 'active text-white fw-semibold' : 'text-light-emphasis'}`}
                                style={{
                                    textDecoration: 'none',
                                    borderBottom: isActive('/') ? '2px solid #fff' : '2px solid transparent',
                                    paddingBottom: '8px',
                                    margin: '0 12px'
                                }}
                                onClick={() => setIsMenuOpen(false)}
                            >
                                Расписание
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link
                                to="/nearest"
                                className={`nav-link ${isActive('/nearest') ? 'active text-white fw-semibold' : 'text-light-emphasis'}`}
                                style={{
                                    textDecoration: 'none',
                                    borderBottom: isActive('/nearest') ? '2px solid #fff' : '2px solid transparent',
                                    paddingBottom: '8px',
                                    margin: '0 12px'
                                }}
                                onClick={() => setIsMenuOpen(false)}
                            >
                                Ближайшие пары
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link
                                to="/teacher"
                                className={`nav-link ${isActive('/teacher') ? 'active text-white fw-semibold' : 'text-light-emphasis'}`}
                                style={{
                                    textDecoration: 'none',
                                    borderBottom: isActive('/teacher') ? '2px solid #fff' : '2px solid transparent',
                                    paddingBottom: '8px',
                                    margin: '0 12px'
                                }}
                                onClick={() => setIsMenuOpen(false)}
                            >
                                Поиск преподавателя
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link
                                to="/export"
                                className={`nav-link ${isActive('/export') ? 'active text-white fw-semibold' : 'text-light-emphasis'}`}
                                style={{
                                    textDecoration: 'none',
                                    borderBottom: isActive('/export') ? '2px solid #fff' : '2px solid transparent',
                                    paddingBottom: '8px',
                                    margin: '0 12px'
                                }}
                                onClick={() => setIsMenuOpen(false)}
                            >
                                Экспорт
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link
                                to="/prefs"
                                className={`nav-link ${isActive('/prefs') ? 'active text-white fw-semibold' : 'text-light-emphasis'}`}
                                style={{
                                    textDecoration: 'none',
                                    borderBottom: isActive('/prefs') ? '2px solid #fff' : '2px solid transparent',
                                    paddingBottom: '8px',
                                    margin: '0 12px'
                                }}
                                onClick={() => setIsMenuOpen(false)}
                            >
                                Настройки
                            </Link>
                        </li>

                        <li className="nav-item d-md-none">
                            <a
                                href={`${import.meta.env.VITE_API_SWAGGER_URL}`}
                                className="nav-link text-light-emphasis"
                                style={{
                                    textDecoration: 'none',
                                    paddingBottom: '8px',
                                    margin: '0 12px'
                                }}
                                target="_blank"
                                rel="noopener noreferrer"
                                onClick={() => setIsMenuOpen(false)}
                            >
                                API
                            </a>
                        </li>
                    </ul>

                    <form onSubmit={(e) => e.preventDefault()} className="d-none d-md-flex ms-3">
                        <a
                            href={`${import.meta.env.VITE_API_SWAGGER_URL}`}
                            className="btn btn-outline-success"
                            target="_blank"
                            rel="noopener noreferrer"
                        >
                            API
                        </a>
                    </form>
                </div>
            </div>
        </nav>
    );
}