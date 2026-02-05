import useDate from "../../hooks/useDate";
import DatePicker from "../DatePicker";
import SchedulePair from "./SchedulePair";
import "./ScheduleSection.css"
import { useCallback, useEffect, useState } from "react";

export default function ScheduleSection({ defaultGroup }) {
    const [group, setGroup] = useState(defaultGroup)
    const [groupsList, setGroupsList] = useState()
    const [schedule, setSchedule] = useState()
    const [loading, setLoading] = useState(false)
    const date = useDate()
    const [invalidGroup, setInvalidGroup] = useState(false)

    // запрос списка групп
    const fetchGroupsList = useCallback(async () => {
        const groupsListResponse = await fetch("Groups")
        if (groupsListResponse.status == 200) {
            const groups = await groupsListResponse.json()
            setGroupsList(groups)
        }
    }, [])

    // запрос расписания группы
    const fetchSchedule = useCallback(async () => {
        if (group.length != 0) {
            setLoading(true)
            try {
                let query = `Schedule/${group}?dateString=${(new Date(date.value)).toISOString().slice(0, 10)}`
                const scheduleResponse = await fetch(query)
                if (scheduleResponse.status == 200) {
                    const scheduleRecords = await scheduleResponse.json()
                    setSchedule(scheduleRecords)
                }
            } finally {
                setLoading(false)
            }
        }
    }, [group, date.value])

    // получение списка групп при старте
    useEffect(() => {
        fetchGroupsList()
    }, [])

    useEffect(() => {
        if (groupsList === undefined) return;

        const currentGroup = group || defaultGroup;

        if (currentGroup && currentGroup.length !== 0 && groupsList.includes(currentGroup)) {
            fetchSchedule();
            setInvalidGroup(false);
        } else {
            setSchedule(undefined);
            setInvalidGroup(!!(currentGroup && currentGroup.length !== 0));
        }
    }, [group, date.value, groupsList, defaultGroup]);

    function keyExtractor(couple) {
        return `${couple.name}-${couple.teacher}-${couple.order}-${couple.auditorium}-${couple.group}`;
    }

    return (
        <section className="schedule_section">
            {groupsList != undefined &&
                <>
                    <div>
                        <input
                            className={`schedule-form-in ${invalidGroup ? "error-input" : ""}`}
                            type="text"
                            list="groups"
                            value={group}
                            placeholder="Учебная группа"
                            onChange={(event) => setGroup(event.target.value)} />
                        <datalist id="groups">
                            {groupsList.map(g => <option key={g}>{g}</option>)}
                        </datalist>
                    </div>
                    <DatePicker {...date} />
                </>
            }

            {loading && (
                <div className="spinner">
                    <div className="spinner-circle"></div>
                </div>
            )}

            {!loading && schedule != undefined &&
                schedule.map(c =>
                    <SchedulePair key={keyExtractor(c)} {...c}></SchedulePair>
                )
            }

            {
                !loading && schedule != undefined && schedule.length == 0 &&
                <h2>Нет занятий</h2>
            }
        </section>
    )
}