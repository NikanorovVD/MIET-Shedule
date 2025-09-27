import { Link } from 'react-router-dom';

export default function Header() {
    return (
        <nav className="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
            <div className="container-fluid">
                <img style={{ height: "50px", paddingRight: "10px" }} src="miet.ico"></img>
                <a className="navbar-brand">MIET Shedule</a>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarCollapse">
                    <ul className="navbar-nav me-auto mb-2 mb-md-0">
                        <li className="nav-link" aria-current="page"  >
                            <Link to="/" style={{ color: 'white', textDecoration: 'none' }}>Расписание</Link>
                        </li>
                        <li className="nav-link" aria-current="page" >
                            <Link to="/teacher" style={{ color: 'white', textDecoration: 'none' }}>Поиск преподавателя</Link>
                        </li>
                        <li className="nav-link" aria-current="page" >
                            <Link to="/export" style={{ color: 'white', textDecoration: 'none' }}>Экспорт</Link>
                        </li>

                    </ul>
                    <form onSubmit={(e) => e.preventDefault()} className="d-flex">
                        <button className="btn btn-outline-success" >API</button>
                    </form>
                </div>
            </div>
        </nav>
    )
}