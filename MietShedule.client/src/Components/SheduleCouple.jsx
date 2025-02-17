import "./SheduleCouple.css"

export default function SheduleCouple({ order, name, teacher, time, auditorium, ...props }) {
    return (
        <table {...props} className="couple">
            <tbody>
                <tr>
                    <td className="order2">{order}</td>
                    <td className="name">{name}
                        <div>
                            <p className="teacher">{teacher}</p>
                            <p className="auditorium">{auditorium}</p>
                            <p className="time">{time}</p>
                        </div>
                    </td>
                </tr>

            </tbody>
        </table>
    )
}