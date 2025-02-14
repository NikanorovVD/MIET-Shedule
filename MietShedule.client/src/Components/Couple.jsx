import "./Couple.css"

export default function Couple({ order, name, teacher, time, auditorium, date, group, ...props }) {
    return (
        <table {...props} className="couple">
            <tbody>
                <tr>
                    <td className="date">{date}</td>
                    <td className="name">{name}</td>
                </tr>
                <tr>
                    <td rowSpan={2} className="order">{order}</td>
                    <td className="group">{group}</td>
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