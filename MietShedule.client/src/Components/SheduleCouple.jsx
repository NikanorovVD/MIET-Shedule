import "./SheduleCouple.css"

export default function SheduleCouple({ order, name, teacher, time, auditorium, ...props }) {
    return (
        <table {...props} className="couple">
            <tbody>
                <tr>
                    <td rowSpan={2} className="order">{order}</td>
                    <td className="name">{name}</td>
                </tr>
                <tr>
                    <td className="teacher">{teacher}</td>
                </tr>
                <tr>
                    <td className="time">{time}</td>
                    <td className="auditorium">{auditorium}</td>
                </tr>

            </tbody>
        </table>
    )
}