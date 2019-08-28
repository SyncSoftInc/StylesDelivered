// �ж϶����Ƿ�Ϊ��
$.isNW = function (obj) {
    return obj === undefined || obj === null || obj.toString().trim().length === 0;
};

// ��ʽ����ʱ��
$.timeFormat = function (value, timeformat) {
    if (!$.isNW(value)) {
        timeformat = timeformat || "MM/DD/YYYY hh:mm:ss A";

        var date = new moment.utc(value).tz('America/Los_Angeles');
        return date.format(timeformat);
    }
};

// ��ʽ��������
$.dateFormat = function (value, timeformat) {
    if (!$.isNW(value)) {
        timeformat = timeformat || "MM/DD/YYYY";
        var date = new moment.utc(value);
        return date.format(timeformat);
    }
};

// �ж���Ϣ���Ƿ�ɹ�
$.isSuccess = function (msgCode) {
    return msgCode === "";
};